using SurvivalTemplatePro;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using V7G.Console;

namespace V7G
{
    [Console("Spawn Command (Survival Template Pro)", "Spawn Command", ConsoleAttributeTarget.Command)]
    public class SpawnCommand : ConsoleCommandBase
    {
        [System.Serializable]
        public class Database
        {
            public GameObject Prefab;
            public int ID;

            public Database(GameObject prefab, int iD)
            {
                Prefab = prefab;
                ID = iD;
            }
        }

        public List<Database> ItemsList = new();

        private void Awake()
        {
            InitCommand<int, int>("spawn", "Spawn item by id next to player", "spawn <id> <amount>", (x, y) => { SpawnItem(x, y); });
        }

        private void SpawnItem(int x, int y)
        {
            if(Player.LocalPlayer != null)
            {
                var player = Player.LocalPlayer;

                var item = ItemsList.Find(o => o.ID == x);

                if(item != null)
                {
                    StartCoroutine(Instantiate(player, item, y));
                }
            }
        }

        private IEnumerator Instantiate(Player player, Database item,int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                Instantiate(item.Prefab, new Vector3(player.transform.position.x, player.transform.position.y + 2, player.transform.position.z + 2), Quaternion.identity);
                yield return new WaitForSeconds(3);
            }
        }
    }
}