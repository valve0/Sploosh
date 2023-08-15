namespace Sploosh.Resources
{
    public class UserSettings
    {
		private bool _soundEffectsStatus;
        private bool _restartGameTrigger;

        public UserSettings()
		{
			_soundEffectsStatus = false;
            _restartGameTrigger = false;

        }

        public bool SoundEffectsStatus
        {
            get { return _soundEffectsStatus; }
        }


        public bool RestartGameTrigger
        {
            get { return _restartGameTrigger; }
            
        }


        public void UpdateSoundEffectStatus()
        {
            _soundEffectsStatus = !_soundEffectsStatus;
        }

        public void UpdateRestartGameTrigger()
        {
            _restartGameTrigger = !_restartGameTrigger;
        }



    }
}
