namespace Ironbug.HVAC
{
    public interface IIB_Curve2D
    {
        void GetMinMax(out double minX, out double maxX);
        double Compute(double x);
    }
}
