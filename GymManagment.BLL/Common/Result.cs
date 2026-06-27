
namespace GymManagment.BLL.Common

{

    public sealed record Result(bool success , string? error = null , ResultKind Kind = ResultKind.OK)

    {
        public static Result Ok() => new(true);

        public static Result  Fail(string error , ResultKind kind = ResultKind.Conflict ) => new (false, error, kind);
        public static Result  NotFound(string error ="NotFound" ) => new (false, error, ResultKind.NotFound);
        public static Result Validation(string error)=> new(false, error,ResultKind.ValidationFailed);

    }
    public sealed record Result<T>(bool success, T? Value, string? error = null, ResultKind Kind = ResultKind.OK)
    {

        public static Result<T> OK(T value) => new(true, value);

        public static Result<T> Fail(string error, ResultKind kind = ResultKind.Conflict) => new(false, default, error, kind);

        public static Result<T> NotFound ( string error = "NotFound" ) => new (false, default, error, ResultKind.NotFound);



    }


}
