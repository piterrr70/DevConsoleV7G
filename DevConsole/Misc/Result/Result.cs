
namespace V7G.Console
{
    public class Result<T>
    {
        public T result;
        public Error error;

        public Result(T result, Error error = null)
        {
            this.result = result;
            this.error = error;
        }
    }
}