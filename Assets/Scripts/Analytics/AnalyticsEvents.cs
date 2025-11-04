using System;
using Unity.Services.Analytics;

/// <summary>
/// Evento cuando el jugador pisa una plataforma
/// </summary>
public class PlatformSteppedOnEvent : Event
{
    public PlatformSteppedOnEvent() : base("platform_stepped_on") { }

    public string PlayerId { set { SetParameter("player_id", value); } }
    public string PlatformId { set { SetParameter("platform_id", value); } }
    public string PlatformType { set { SetParameter("platform_type", value); } }
    public int CurrentAttemptNumber { set { SetParameter("current_attempt_number", value); } }
    public int LivesRemaining { set { SetParameter("lives_remaining", value); } }
    public int RunTimeSeconds { set { SetParameter("run_time_seconds", value); } }
    public int CheckpointTimeSeconds { set { SetParameter("checkpoint_time_seconds", value); } }
    public int SessionTimeSeconds { set { SetParameter("session_time_seconds", value); } }
    public string ZoneId { set { SetParameter("zone_id", value); } }
}

/// <summary>
/// Evento cuando el jugador muere
/// </summary>
public class PlayerDiedEvent : Event
{
    public PlayerDiedEvent() : base("player_died_v2") { }

    public string PlayerId { set { SetParameter("player_id", value); } }
    public string DeathCause { set { SetParameter("death_cause", value); } }
    public float DeathPositionX { set { SetParameter("death_position_x", value); } }
    public float DeathPositionY { set { SetParameter("death_position_y", value); } }
    public float DeathPositionZ { set { SetParameter("death_position_z", value); } }
    public string ZoneId { set { SetParameter("zone_id", value); } }
    public int CurrentAttemptNumber { set { SetParameter("current_attempt_number", value); } }
    public int RunTimeSeconds { set { SetParameter("run_time_seconds", value); } }
    public int CheckpointTimeSeconds { set { SetParameter("checkpoint_time_seconds", value); } }
    public int SessionTimeSeconds { set { SetParameter("session_time_seconds", value); } }
    public string LastCheckpointId { set { SetParameter("last_checkpoint_id", value); } }
}

/// <summary>
/// Evento cuando el jugador alcanza un checkpoint
/// </summary>
public class CheckpointReachedEvent : Event
{
    public CheckpointReachedEvent() : base("checkpoint_reached") { }

    public string PlayerId { set { SetParameter("player_id", value); } }
    public string CheckpointId { set { SetParameter("checkpoint_id", value); } }
    public string ZoneId { set { SetParameter("zone_id", value); } }
    public int CurrentAttemptNumber { set { SetParameter("current_attempt_number", value); } }
    public int LivesRemaining { set { SetParameter("lives_remaining", value); } }
    public int TotalAttemptsInZone { set { SetParameter("total_attempts_in_zone", value); } }
    public int CheckpointTimeSeconds { set { SetParameter("checkpoint_time_seconds", value); } }
    public int SessionTimeSeconds { set { SetParameter("session_time_seconds", value); } }
    public string PreviousCheckpointId { set { SetParameter("previous_checkpoint_id", value); } }
}

/// <summary>
/// Evento cuando el jugador completa una zona
/// </summary>
public class ZoneWinReachedEvent : Event
{
    public ZoneWinReachedEvent() : base("zone_win_reached") { }

    public string PlayerId { set { SetParameter("player_id", value); } }
    public string ZoneId { set { SetParameter("zone_id", value); } }
    public int TotalAttemptsInZone { set { SetParameter("total_attempts_in_zone", value); } }
    public int TotalDeathsInZone { set { SetParameter("total_deaths_in_zone", value); } }
    public float WinCompletionTimeSeconds { set { SetParameter("win_completion_time_seconds", value); } }
    public int TotalCheckpointsInZone { set { SetParameter("total_checkpoints_in_zone", value); } }
    public string SessionId { set { SetParameter("session_id", value); } }
    public int SessionTimeInZoneSeconds { set { SetParameter("session_time_in_zone_seconds", value); } }
}

/// <summary>
/// Evento cuando termina una sesion
/// </summary>
public class SessionEndedEvent : Event
{
    public SessionEndedEvent() : base("session_ended") { }

    public string PlayerId { set { SetParameter("player_id", value); } }
    public string SessionId { set { SetParameter("session_id", value); } }
    public float SessionDurationMinutes { set { SetParameter("session_duration_minutes", value); } }
    public int TotalAttemptsInSession { set { SetParameter("total_attempts_in_session", value); } }
    public string ZonesAttemptedList
    {
        set
        {
            // Convertir array a string separado por comas para Unity Analytics
            string zonesString = value != null ? string.Join(",", value) : "";
            SetParameter("zones_attempted_list", zonesString);
        }
    }
    public string FurthestCheckpointReached { set { SetParameter("furthest_checkpoint_reached", value); } }
}

/// <summary>
/// Evento cuando el jugador inicia un run
/// </summary>
public class RunStartedEvent : Event
{
    public RunStartedEvent() : base("run_started") { }

    public string PlayerId { set { SetParameter("player_id", value); } }
    public string ZoneId { set { SetParameter("zone_id", value); } }
    public int CurrentAttemptNumber { set { SetParameter("current_attempt_number", value); } }
    public string SessionId { set { SetParameter("session_id", value); } }
}

/// <summary>
/// Evento cuando el jugador sale de un run sin completarlo
/// </summary>
public class RunExitedEvent : Event
{
    public RunExitedEvent() : base("run_exited") { }

    public string PlayerId { set { SetParameter("player_id", value); } }
    public string ZoneId { set { SetParameter("zone_id", value); } }
    public int CurrentAttemptNumber { set { SetParameter("current_attempt_number", value); } }
    public int RunTimeSeconds { set { SetParameter("run_time_seconds", value); } }
    public string FurthestCheckpointReached { set { SetParameter("furthest_checkpoint_reached", value); } }
}
