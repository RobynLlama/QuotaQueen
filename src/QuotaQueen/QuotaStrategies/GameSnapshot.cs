using YAPYAP;

namespace QuotaQueen.QuotaStrategies;

/// <summary>
/// A snapshot of the GameManager at the moment a quota was requested
/// </summary>
/// <param name="instance">The GameManager instance</param>
/// <param name="quotaEnded">If the current quota has ended</param>
public readonly struct GameSnapshot(GameManager instance, bool quotaEnded = false)
{
  /// <summary>
  /// Has the current quota ended. If true, this quota request
  /// is probably coming from the judgement scene with the Moon
  /// Wizard
  /// </summary>
  public readonly bool QuotaJustEnded = quotaEnded;

  /// <summary>
  /// The total quotas completed, excluding the current quota if
  /// it just completed and is in the judgement scene
  /// </summary>
  public readonly int QuotasCompleted = instance.quotaSessionsCompleted;

  /// <summary>
  /// The count of players currently in the session
  /// </summary>
  public readonly int PlayersInSession = instance.playersByActorId.Keys.Count;

  /// <summary>
  /// The quota goal for the current quota, prior to this update
  /// </summary>
  public readonly int CurrentQuotaGoal = instance.currentQuotaScoreGoal;

  /// <summary>
  /// The multiplier used to modify quotas. In the base game this
  /// is used after day 5 because the first 5 days are hard-coded
  /// to specific values
  /// </summary>
  public readonly float QuotaMultiplier = instance.sessionQuotaMultiplier;

  /// <summary>
  /// Returns the effective quota count. This includes the current
  /// completed quota if the game is in the judgement scene and has
  /// <summary>
  /// Returns a very lovely formatted string with all members showing
  /// </summary>
  /// <returns></returns>
  public override string ToString()
  {
    StringBuilder sb = new("GameState:\n");
    sb.AppendLine($"  Just Ended:         {QuotaJustEnded}");
    sb.AppendLine($"  Completed:          {QuotasCompleted}");
    sb.AppendLine($"  Players:            {PlayersInSession}");
    sb.AppendLine($"  Current Goal:       {CurrentQuotaGoal}");
    sb.AppendLine($"  Multiplier:         {QuotaMultiplier}");
    sb.AppendLine($"  Effective Quotas:   {EffectiveQuotaCount}");

    return sb.ToString();
  }
}
