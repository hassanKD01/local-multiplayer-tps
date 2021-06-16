using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Gun : MonoBehaviour
{
     float damage = 10f;
    float range = 100f;
    float fireRate = 0.25f;

    public Camera fpscam;
    public ParticleSystem flash;
    public Image damageImage;
    public TextMeshProUGUI healthText;

    string fire;
    private float r, g, b, a;
    private float nextTimeToFire = 0f;

    private void Start()
    {
        r = damageImage.color.r;
        g = damageImage.color.g;
        b = damageImage.color.b;
        a = damageImage.color.a;
        if (gameObject.name.Equals("Player1Gun")) fire = "Fire1";
        else fire = "FireAlt";
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton(fire) && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + fireRate;
            Shoot();
        }
        if(a > 0)
        {
            a -= 0.05f;
            a = Mathf.Clamp(a, 0, 1f);
            AdjustColor();
        }
    }
    
    public void Shoot()
    {
        flash.Play();
        AudioSource audio = GetComponent<AudioSource>();
        audio.Play();
        RaycastHit hit;
        if(Physics.Raycast(fpscam.transform.position, fpscam.transform.forward, out hit, range))
        {
            Animation2DStateController target = hit.transform.GetComponent<Animation2DStateController>();
            if (target != null)
            {
                target.TakeDamage(damage);
                a += 0.5f;
                float health = Int64.Parse(healthText.text);
                health -= damage;
                healthText.text = ""+health;
            }
        }
    }

    private void AdjustColor()
    {
        Color c = new Color(r, g, b, a);
        damageImage.color = c;
    }
}
