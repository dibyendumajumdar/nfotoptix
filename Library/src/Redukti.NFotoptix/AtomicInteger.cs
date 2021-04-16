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
            throw new System.NotImplementedException();
        }
    }
}