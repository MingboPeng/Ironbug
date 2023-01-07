namespace Ironbug.HVAC
{
    public interface IIB_Curve3D
    {
        void GetMinMax(out double minX, out double maxX, out double minY, out double maxY);
        double Compute(double x, double y);
    }
}
