using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Behavior
{
    public class ValidationBehavior<TRequest, TRespone> : IPipelineBehavior<TRequest, TRespone>
      where TRequest : IRequest<TRespone>
    {
        private readonly IEnumerable<IValidator<TRequest>> validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            this.validators = validators;
        }
      

        public async Task<TRespone> Handle(TRequest request, RequestHandlerDelegate<TRespone> next, CancellationToken cancellationToken)
        {
            if (validators.Any())
            {
                var validationContext = new ValidationContext<TRequest>(request);

                var validationResult = await Task.WhenAll(validators.Select(x => x.ValidateAsync(validationContext, cancellationToken)));
                var errors = validationResult.SelectMany(x => x.Errors).Where(x => x != null).ToList();
                if (errors.Count != 0)
                {
                    //string error = "";
                    //foreach (var item in errors)
                    //    error += item.ErrorMessage + "\n";
                    throw new ValidationException(errors);
                }
            }
            return await next();
        }



        //public async Task<TRespone> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TRespone> next)
        //{
        //    if (validators.Any())
        //    {
        //        var validationContext = new ValidationContext<TRequest>(request);

        //        var validationResult = await Task.WhenAll(validators.Select(x => x.ValidateAsync(validationContext, cancellationToken)));
        //        var errors = validationResult.SelectMany(x => x.Errors).Where(x => x != null).ToList();
        //        if (errors.Count != 0)
        //        {
        //            //string error = "";
        //            //foreach (var item in errors)
        //            //    error += item.ErrorMessage + "\n";
        //            throw new ValidationException(errors);
        //        }
        //    }
        //    return await next();
        //}
    }
}
