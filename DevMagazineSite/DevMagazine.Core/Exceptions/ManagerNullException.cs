using System;
using System.Linq;

namespace DevMagazine.Core.Exceptions
{
    /// <summary>
    /// A special exception that deals with Sitefinity manager base classes
    /// It should be thrown in the event of a Sitefinity manager being null
    /// </summary>
    [Serializable]
    public class ManagerNullException : Exception
    {
        #region Constructors

        public ManagerNullException(Type managerType)
            : base()
        {
            this.managerType = managerType;
        }

        public ManagerNullException(string message, Type managerType)
            : base(message)
        {
            this.managerType = managerType;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the manger type causing the exception
        /// </summary>
        public Type ManagerType
        {
            get
            {
                return this.managerType;
            }
            set
            {
                this.managerType = value;
            }
        }

        #endregion


        #region Private fields and constants

        private Type managerType;

        #endregion
    }
}
