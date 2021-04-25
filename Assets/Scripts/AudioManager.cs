using UnityEngine;

public class AudioManager : MonoBehaviour {
    public GameObject soundEffectPlayerPrefab;
    
    public AudioClip pickupCoinSound;
    public AudioClip[] explosionSounds;
    public AudioClip shootingGunSound;

    private AudioSource playerAudioSource;

    private float pickupCoinPitch = 1f;
    private Timer pickupCoinTimer = new Timer();

    private void Start() {
        playerAudioSource = GameObject.Find("Player").GetComponent<AudioSource>();
        pickupCoinTimer.Set(0);
    }

    private void FixedUpdate() {
        pickupCoinTimer.Update();
    }

    private AudioSource CreateSoundEffectPlayerAt(Vector2 position) {
        var go = Instantiate(soundEffectPlayerPrefab, position, Quaternion.identity, transform);
        return go.GetComponent<AudioSource>();
    }
    
    public void PlayPickupCoinSound() {
        if (pickupCoinTimer.Check()) {
            pickupCoinPitch = 1f;
        }
        
        playerAudioSource.pitch = pickupCoinPitch;
        playerAudioSource.volume = 0.75f;
        playerAudioSource.PlayOneShot(pickupCoinSound);
        
        pickupCoinPitch += 0.05f;
        pickupCoinTimer.Set(3000);
    }

    public void PlayExplosionSound(Vector2 position) {
        var audioSource = CreateSoundEffectPlayerAt(position);

        var clip = explosionSounds[Random.Range(0, explosionSounds.Length)];
        
        audioSource.pitch = 1f;
        audioSource.volume = 1f;
        audioSource.PlayOneShot(clip);
    }

    public void PlayShootingGunSound(Vector2 position) {
        var audioSource = CreateSoundEffectPlayerAt(position);
        
        audioSource.pitch = Random.Range(1f, 1.5f);
        audioSource.volume = 0.5f;
        audioSource.PlayOneShot(shootingGunSound);
    }
    
    public void PlayPlayerShootingGunSound() {
        playerAudioSource.pitch = Random.Range(1f, 1.5f);
        playerAudioSource.volume = 0.5f;
        playerAudioSource.PlayOneShot(shootingGunSound);
    }
}