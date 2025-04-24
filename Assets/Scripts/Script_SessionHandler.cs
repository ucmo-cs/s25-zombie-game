using Unity.Services.Lobbies;
using Unity.Services.Multiplayer;
using UnityEngine;

public class Script_SessionHandler : MonoBehaviour
{
    private ISession activeSession = null;

    public static Script_SessionHandler Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
        }
    }

    public void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void SetActiveSession(ISession session)
    {
        activeSession = session;
    }

    public ISession GetActiveSession()
    {
        return activeSession;
    }

    public void LeaveActiveSession()
    {
        if (activeSession != null)
        {
            try
            {
                activeSession.LeaveAsync();
            }
            catch
            {
                // Ignored as we are exiting the game
            }
            finally
            {
                activeSession = null;
            }
        }
    }

    public void LockSession()
    {
        LobbyService.Instance.UpdateLobbyAsync(activeSession.Id, new UpdateLobbyOptions { IsLocked = true });
    }
}
