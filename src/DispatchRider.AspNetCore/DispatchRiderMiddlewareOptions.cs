namespace DispatchRider.AspNetCore
{
    public class DispatchRiderMiddlewareOptions
    {
        public BaseContextExceptionFilter ContextExceptionFilter { get; set; }
        public DispatchRiderMiddlewareOptions()
        {
            ContextExceptionFilter = new BaseContextExceptionFilter();
        }
    }
}