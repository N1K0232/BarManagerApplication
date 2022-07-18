using System.Data;

namespace BackendGestionaleBar.DataAccessLayer;

public interface ISqlContext
{
    Task<IEnumerable<T>> GetDataAsync<T>(string sql, object param = null, IDbTransaction transaction = null, CommandType? commandType = null)
        where T : class;

    Task<IEnumerable<TReturn>> GetDataAsync<TFirst, TSecond, TReturn>(string sql, Func<TFirst, TSecond, TReturn> map, object param = null, IDbTransaction transaction = null, CommandType? commandType = null, string splitOn = "Id")
        where TFirst : class
        where TSecond : class
        where TReturn : class;

    Task<IEnumerable<TReturn>> GetDataAsync<TFirst, TSecond, TThird, TReturn>(string sql, Func<TFirst, TSecond, TThird, TReturn> map, object param = null, IDbTransaction transaction = null, CommandType? commandType = null, string splitOn = "Id")
        where TFirst : class
        where TSecond : class
        where TThird : class
        where TReturn : class;

    Task<IEnumerable<TReturn>> GetDataAsync<TFirst, TSecond, TThird, TFourth, TReturn>(string sql, Func<TFirst, TSecond, TThird, TFourth, TReturn> map, object param = null, IDbTransaction transaction = null, CommandType? commandType = null, string splitOn = "Id")
        where TFirst : class
        where TSecond : class
        where TThird : class
        where TFourth : class
        where TReturn : class;

    Task<T> GetObjectAsync<T>(string query, object param = null, IDbTransaction transaction = null, CommandType? commandType = null)
        where T : class;

    Task<TReturn> GetObjectAsync<TFirst, TSecond, TReturn>(string sql, Func<TFirst, TSecond, TReturn> map, object param = null, IDbTransaction transaction = null, CommandType? commandType = null, string splitOn = "Id")
        where TFirst : class
        where TSecond : class
        where TReturn : class;

    Task<TReturn> GetObjectAsync<TFirst, TSecond, TThird, TReturn>(string sql, Func<TFirst, TSecond, TThird, TReturn> map, object param = null, IDbTransaction transaction = null, CommandType? commandType = null, string splitOn = "Id")
        where TFirst : class
        where TSecond : class
        where TThird : class
        where TReturn : class;

    Task<TReturn> GetObjectAsync<TFirst, TSecond, TThird, TFourth, TReturn>(string sql, Func<TFirst, TSecond, TThird, TFourth, TReturn> map, object param = null, IDbTransaction transaction = null, CommandType? commandType = null, string splitOn = "Id")
        where TFirst : class
        where TSecond : class
        where TThird : class
        where TFourth : class
        where TReturn : class;

    Task<T> GetSingleValueAsync<T>(string sql, object param = null, IDbTransaction transaction = null, CommandType? commandType = null);

    Task<int> ExecuteAsync(string sql, object param = null, IDbTransaction transaction = null, CommandType? commandType = null);

    IDbTransaction BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.Unspecified);

    Task<IDbTransaction> BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.Unspecified);
}