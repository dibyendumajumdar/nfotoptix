namespace Redukti.Nfotopix
{
    public class AtomicInteger
    {
        private int _i;

        public AtomicInteger(int i)
        {
            this._i = i;
        }
        
        public int incrementAndGet()
        {
            _i++;
            return _i;
        }
    }
}