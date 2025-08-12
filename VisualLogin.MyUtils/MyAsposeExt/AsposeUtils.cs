using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace VisualLogin.MyUtils.MyAsposeExt
{
    public static class AsposeUtils
    {
        private static bool _asposeCellsAvailable;
        private static Assembly _asposeCellsAssembly;
        private static Type _workbookType;
        private static Type _worksheetType;
        private static Type _cellsType;
        private static Type _importTableOptionsType;

        /// <summary>
        /// 动态加载Aspose.Cells程序集
        /// </summary>
        /// <returns></returns>
        private static bool LoadAsposeCellsAssembly()
        {
            try
            {
                // 检查是否已经加载
                var assemblies = AppDomain.CurrentDomain.GetAssemblies();
                var loadedAssembly = assemblies.FirstOrDefault(a =>
                    a.GetName().Name.Equals("Aspose.Cells", StringComparison.OrdinalIgnoreCase));

                if (loadedAssembly != null)
                {
                    return InitializeTypes(loadedAssembly);
                }

                // 尝试从常见路径加载
                string[] searchPaths = {
                AppDomain.CurrentDomain.BaseDirectory,
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin"),
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "lib"),
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assemblies"),
                // 当前目录的所有子目录
            };

                foreach (string basePath in searchPaths)
                {
                    if (Directory.Exists(basePath))
                    {
                        // 搜索Aspose.Cells.dll
                        string[] dllPaths = Directory.GetFiles(basePath, "Aspose.Cells.dll", SearchOption.AllDirectories);

                        foreach (string dllPath in dllPaths)
                        {
                            try
                            {
                                var assembly = Assembly.LoadFrom(dllPath);
                                if (assembly.GetName().Name.Equals("Aspose.Cells", StringComparison.OrdinalIgnoreCase))
                                {
                                    return InitializeTypes(assembly);
                                }
                            }
                            catch
                            {
                                // 继续尝试其他路径
                                continue;
                            }
                        }
                    }
                }

                // 尝试从GAC加载
                try
                {
                    var assembly = Assembly.Load("Aspose.Cells");
                    return InitializeTypes(assembly);
                }
                catch
                {
                    // GAC加载失败
                }

                return false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"加载Aspose.Cells时发生错误: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 初始化类型
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        private static bool InitializeTypes(Assembly assembly)
        {
            try
            {
                _workbookType = assembly.GetType("Aspose.Cells.Workbook");
                _worksheetType = assembly.GetType("Aspose.Cells.Worksheet");
                _cellsType = assembly.GetType("Aspose.Cells.Cells");
                _importTableOptionsType = assembly.GetType("Aspose.Cells.ImportTableOptions");

                if (_workbookType != null && _worksheetType != null &&
                    _cellsType != null && _importTableOptionsType != null)
                {
                    _asposeCellsAssembly = assembly;
                    _asposeCellsAvailable = true;
                    return true;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"初始化Aspose.Cells类型时发生错误: {ex.Message}");
            }

            return false;
        }

        /// <summary>
        /// 检查Aspose.Cells是否可用（会尝试动态加载）
        /// </summary>
        /// <returns></returns>
        public static bool IsAsposeCellsAvailable()
        {
            if (!_asposeCellsAvailable)
            {
                _asposeCellsAvailable = LoadAsposeCellsAssembly();
            }
            return _asposeCellsAvailable;
        }

        /// <summary>
        /// 导出对象列表到Excel文件
        /// </summary>
        public static bool ToAsposeExcel<T>(this IList<T> data, string filePath)
        {
            if (!IsAsposeCellsAvailable())
            {
                throw new InvalidOperationException("Aspose.Cells library is not available.");
            }

            if (data == null)
                throw new ArgumentNullException(nameof(data));

            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentNullException(nameof(filePath));

            object workbook = null;
            try
            {
                // 创建Workbook实例
                workbook = Activator.CreateInstance(_workbookType);

                // 获取Worksheets[0]
                PropertyInfo worksheetsProperty = _workbookType.GetProperty("Worksheets");
                object worksheets = worksheetsProperty?.GetValue(workbook);
                var methods = worksheets?.GetType().GetMethods().Where(m => m.Name == "get_Item")
                    .ToArray();
                var getItemMethod = methods?.FirstOrDefault(m =>
                    m.GetParameters().Length == 1 &&
                    m.GetParameters()[0].ParameterType == typeof(int));
                object worksheet = getItemMethod?.Invoke(worksheets, new object[] { 0 });

                // 获取Cells
                PropertyInfo cellsProperty = _worksheetType.GetProperty("Cells");
                object cells = cellsProperty?.GetValue(worksheet);

                // 创建ImportTableOptions实例
                object importTableOptions = Activator.CreateInstance(_importTableOptionsType);

                // 设置IsFieldNameShown = true
                PropertyInfo isFieldNameShownProperty = _importTableOptionsType.GetProperty("IsFieldNameShown");
                isFieldNameShownProperty?.SetValue(importTableOptions, true);


                // 调用ImportCustomObjects方法
                MethodInfo importMethod = _cellsType.GetMethod("ImportCustomObjects",
                    new[] { typeof(System.Collections.ICollection), typeof(int), typeof(int), _importTableOptionsType });

                if (importMethod != null)
                {
                    importMethod.Invoke(cells, new object[] { data, 0, 0, importTableOptions });
                }
                else
                {
                    throw new InvalidOperationException("无法找到ImportCustomObjects方法");
                }

                // 保存文件
                MethodInfo saveMethod = _workbookType.GetMethod("Save", new[] { typeof(string) });
                saveMethod?.Invoke(workbook, new object[] { filePath });

                return true;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to export data to Excel: {ex.Message}", ex);
            }
            finally
            {
                // 确保资源被释放
                if (workbook != null)
                {
                    try
                    {
                        MethodInfo disposeMethod = _workbookType.GetMethod("Dispose");
                        disposeMethod?.Invoke(workbook, null);
                    }
                    catch
                    {
                        // 忽略释放时的异常
                    }
                }
            }
        }

   }
}