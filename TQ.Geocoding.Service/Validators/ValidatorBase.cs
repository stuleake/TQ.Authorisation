using System;
using TQ.Geocoding.Service.Interface.Validators;

namespace TQ.Geocoding.Service.Validators
{
    public abstract class ValidatorBase<TEntity> : IValidator where TEntity : IComparable
    {
        protected TEntity Context { get; private set; }

        protected ValidatorBase(TEntity context)
        {
            Context = context;
        }

        public void Validate()
        {
            if (!IsValid)
            {
                throw new Exception(ErrorMessage);
            }
        }

        public abstract bool IsValid { get; }
        public abstract string ErrorMessage { get; }
    }
}
