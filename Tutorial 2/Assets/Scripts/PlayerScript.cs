using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{

    private Rigidbody2D rd2d;
    public float speed;
    public Text score;
    public Text lives;
    private int scoreValue = 0;
    public float jumpforce = 2.5f;
    private int lifeValue;
    public GameObject winTextObject;
    public GameObject loseTextObject;
    public AudioSource musicSource;
    public AudioClip musicClipOne;
    public AudioClip musicClipTwo;
    public AudioClip musicClipThree;
    public bool facingRight = true;
    Animator anim;




    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;
        lifeValue = 3;

        rd2d = GetComponent<Rigidbody2D>();
        score.text = "Score: " + scoreValue.ToString();
        lives.text = "Lives: " + lifeValue.ToString();
        winTextObject.SetActive(false);
        loseTextObject.SetActive(false);
        if (sceneName == "Level 1")
        {
            musicSource.clip = musicClipOne;
            musicSource.Play();
        }
        else if (sceneName == "Level 2")
        {
            musicSource.clip = musicClipThree;
            musicSource.Play();
        }

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        anim.SetInteger("State", 0);

        float hozMovement = Input.GetAxis("Horizontal");
        float verMovement = Input.GetAxis("Vertical");

        rd2d.AddForce(new Vector2(hozMovement * speed, verMovement * speed));
        if (hozMovement > 0 && !facingRight)
        {
            Flip();
        }
        else if (hozMovement < 0 && facingRight)
        {
            Flip();
        }
        if (Input.GetAxis("Vertical") != 0)
        {
            anim.SetInteger("State", 2);
        }
        if (Input.GetAxis("Horizontal") != 0 && Input.GetAxis("Vertical") == 0)
        {
            anim.SetInteger("State", 1);
        }
  
       
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;

        if (collision.tag == "Coin")
        {
            Destroy(collision.gameObject);
            scoreValue += 1;
            score.text = "Score: " + scoreValue.ToString();
        }
        if (sceneName == "Level 1" && scoreValue == 4)
        {
            SceneManager.LoadScene(sceneName: "Level 2");
            //winTextObject.SetActive(true);
        }
        else if (sceneName == "Level 2" && scoreValue == 4)
        {
            winTextObject.SetActive(true);
            musicSource.clip = musicClipTwo;
            musicSource.Play();
        }
        if (collision.tag == "Enemy")
        {
            Destroy(collision.gameObject);
            lifeValue -= 1;
            lives.text = "Lives: " + lifeValue.ToString();
            if (lifeValue <= 0)
            {
                GameOver();
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground")
        {
            if(Input.GetKey(KeyCode.W))
            {
                rd2d.AddForce(new Vector2(0, jumpforce), ForceMode2D.Impulse);
            }
        }

    }

    void GameOver()
    {
        loseTextObject.SetActive(true);
        Destroy(GameObject.FindWithTag("Player"));
    }
}
