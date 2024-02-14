using System;

namespace ShowGraphic
{
    static class ScaleGraph
    {
        public static string Scale(double Length, int idx)
        {
            int i = idx % 11;
            //деления на координатных осях
            string[] scale = { "0", "0.1", "0.2", "0.3", "0.4", "0.5", "0.6", "0.7", "0.8", "0.9", "1.0" };
            string SLengthF = (double.Parse(scale[i]) * Length).ToString();
            string[] SS = SLengthF.Split('.');
            string SLength;
            if (SS.Length == 2)
            {
                int LS = SS[1].Length;
                if (LS > 4)
                    LS = 4;
                SLength = SS[0] + "." + SS[1].Substring(0, LS);
            }
            else
                SLength = SS[0];
            return SLength;
        }
        public static string ScaleX0X1(double X0,double X1, int idx)
        {
            double Length = X1 - X0;
            int i = idx % 11;
            //деления на координатных осях
            string[] scale = { "0", "0.1", "0.2", "0.3", "0.4", "0.5", "0.6", "0.7", "0.8", "0.9", "1.0" };
            string SLengthF = (X0 + double.Parse(scale[i]) * Length).ToString();
            string[] SS = SLengthF.Split('.');
            string SLength;
            if (SS.Length == 2)
            {
                int LS = SS[1].Length;
                if (LS > 4)
                    LS = 4;
                SLength = SS[0] + "." + SS[1].Substring(0, LS);
            }
            else
                SLength = SS[0];
            return SLength;
        }
    }
}
