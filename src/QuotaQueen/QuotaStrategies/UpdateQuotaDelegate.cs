namespace QuotaQueen.QuotaStrategies;

/// <summary>
/// A delegate for when the next quota is requested
/// </summary>
/// <param name="gameState">A snapshot of many important values from the moment this quota was requested</param>
/// <returns></returns>
public delegate int UpdateQuotaDelegate(GameSnapshot gameState);
