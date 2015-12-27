using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drone.Core
{
    public static class @enums
    {
        public enum objective { ObjectiveNope, ObjectiveMove, ObjectiveStop };
        public enum action { ActionNo, ActionMoving, ActionCalculating, ActionReturning, ActionBooting, ActionProblem, ActionWaiting};
        public enum control { ControlNo, ControlAuto, ControlMan};
        public enum mode { modeHorizontal, modeVertical, modeHtoV, modeVtoH };
    }
}
