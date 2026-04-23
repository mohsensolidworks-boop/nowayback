namespace Main.Infrastructure.Utils
{
    public class EnableCounter
    {
        private int _counter;

        public bool IsEnabled()
        {
            return _counter == 0;
        }

        public void Enable()
        {
            if (_counter == 0)
            {
                return;
            }
            
            _counter -= 1;
        }

        public void Disable()
        {
            _counter += 1;
        }
        
        public void RestoreDefaults()
        {
            _counter = 0;
        }
    }
}
