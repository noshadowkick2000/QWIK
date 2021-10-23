using System;
using System.Collections.Generic;

namespace Server.Global
{
    public class Party : Singleton<Party>
    {
        private List<Player> players = new List<Player>();

        public Action<List<Player>> onPlayersUpdated;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        public void AddPlayer(Player player)
        {
            players.Add(player);
            onPlayersUpdated?.Invoke(players);
        }

        public void RemovePlayer(Player player)
        {
            players.Remove(player);
            onPlayersUpdated?.Invoke(players);
        }

        public void ResetPlayers()
        {
            players.Clear();
        }

        public List<Player> GetPlayers()
        {
            return players;
        }

        public int GetPartySize()
        {
            return players.Count;
        }

        public List<int> GetPartyIds()
        {
            List<int> ids = new List<int>();
            foreach (var hostPlayer in players)
            {
                ids.Add(hostPlayer.GetId());
            }

            return ids;
        }

        private Player GetPlayerById(int id)
        {
            foreach (var hostPlayer in players)
            {
                if (hostPlayer.GetId() == id)
                    return hostPlayer;
            }

            return null;
        }

        public void GivePoints(List<int> finishingOrder)
        {
            for (int i = 0; i < finishingOrder.Count; i++)
            {
                GetPlayerById(finishingOrder[i]).GivePoints(i);
            }
        }
    }
}
