using UnityEngine;

namespace HighwayPursuit
{
    public class EnemyController : MonoBehaviour
    {
        private float _moveSpeed;
        private EnemyManager _enemyManager;

        private void Update()
        {
            transform.Translate(-transform.forward * _moveSpeed * Random.Range(0.5f, 1f) * Time.deltaTime);

            if (transform.position.z <= -10)
                _enemyManager.DeactivateEnemy(this);
        }

        public void SetDefault(float speed, EnemyManager enemyManager)
        {
            this._moveSpeed = speed;
            this._enemyManager = enemyManager;
        }

        public void ActivateEnemy()
        {
            gameObject.SetActive(true);
        }
    }
}