namespace Drone.Core
{
    public static class @enums
    {
        #region Public Enums

        public enum action
        {
            ActionNo,

            ActionMoving,

            ActionCalculating,

            ActionReturning,

            ActionBooting,

            ActionProblem,

            ActionWaiting
        }

        public enum control
        {
            ControlNo,

            ControlAuto,

            ControlMan
        }

        public enum mode
        {
            modeHorizontal,

            modeVertical,

            modeHtoV,

            modeVtoH
        }

        public enum objective
        {
            ObjectiveNope,

            ObjectiveMove,

            ObjectiveStop
        }

        #endregion Public Enums
    }
}