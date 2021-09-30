using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{

    [SerializeField]
    private float _speed = 8.0f;

    public enum LaserType{
        Player = 0,
        Enemy = 1,
    }

    public LaserType Shooter { get; set; } = LaserType.Player;

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = Shooter == LaserType.Player ? Vector3.up : Vector3.down;
        transform.Translate(direction * _speed * Time.deltaTime);

        if (transform.position.y > 8.5f || transform.position.y < -6.5f)
        {

            if (transform.parent)
            {
                Destroy(transform.parent.gameObject);
            }

            Destroy(this.gameObject);
        }
    }

    
}
