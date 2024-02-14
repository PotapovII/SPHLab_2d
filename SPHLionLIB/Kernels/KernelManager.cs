/////////////////////////////////////////////////////
// дата: 16 03 2019
// автор: Потапов Игорь Иванович 
// библиотека: SPH_LibraryKernels
// лицензия: свободное распространение
/////////////////////////////////////////////////////
using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Numerics;

namespace SPHLionLIB
{
    /// <summary>
    /// Класс для управления коллекцией ядер порожденных от базового класса BaseKernel
    /// </summary>
    public static class KernelManager
    {
        public static string ErrorMessage = "Ok";
        /// <summary>
        /// список потомков базового класса
        /// </summary>
        static Type[] ListModels=null;
        /// <summary>
        /// Строка с имененм пространства имен в котором находится требуемая иерархия классов
        /// </summary>
        static string NameSpace = "SPHLibrary";
        /// <summary>
        /// Получить список потомков
        /// </summary>
        /// <returns>список потомков</returns>
        public static List<string> GetNamesKernels()
        {
            if (ListModels == null)
            {
                try
                {
                    // получение имени пространства имен
                    NameSpace = Assembly.GetExecutingAssembly().GetName().Name;
                    Assembly assembly = System.Reflection.Assembly.Load(NameSpace);
                    Type baseType = typeof(BaseKernel);
                    ListModels = Array.FindAll
                        (
                            assembly.GetTypes(), 
                            delegate(Type type)
                            {
                                return (baseType != type) && baseType.IsAssignableFrom(type);
                            }
                        );
                    ListModels = ListModels.OrderBy(s => s.Name).ToArray();
                }
                catch (Exception e)
                {
                    ErrorMessage = e.Message;
                }
            }
            List<string> Names = new List<string>();
            for (int i = 0; i < ListModels.Length; i++)
                Names.Add(ListModels[i].Name);
            return Names;
        }
        /// <summary>
        /// получить экземпляр указанного класса
        /// </summary>
        /// <param name="Index">порядковый номер в списке потомков базового класса</param>
        /// <returns></returns>
        public static BaseKernel CreateKernel(int Index, float kernelSize, int dimension = 2)
        {
            Type TestType = Type.GetType(ListModels[Index % ListModels.Length].FullName, false, true);
            // ищем конструктор с параметрами
            ConstructorInfo constructor = TestType.GetConstructor(new Type[] { typeof(float), typeof(int) });
            // вызываем конструтор
            return (BaseKernel)constructor.Invoke(new object[] { kernelSize, dimension });
        }


        public static BaseKernel CreateKernel(int Index,  int dimension = 2)
        {
            Type TestType = Type.GetType(ListModels[Index % ListModels.Length].FullName, false, true);
            // ищем конструктор с параметрами
            ConstructorInfo constructor = TestType.GetConstructor(new Type[] {typeof(int) });
            // вызываем конструтор
            return (BaseKernel)constructor.Invoke(new object[] { dimension });
        }
    }
}
