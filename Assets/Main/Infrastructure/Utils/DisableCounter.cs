namespace Main.Infrastructure.Utils
{
    public class DisableCounter
    {
        private int _counter;

        public bool IsEnabled()
        {
            return _counter > 0;
        }

        public void Enable()
        {
            _counter++;
        }

        public void Disable()
        {
            if (_counter == 0)
            {
                return;
            }
            
            _counter--;
        }
        
        public void Reset()
        {
            _counter = 0;
        }
    }
}
