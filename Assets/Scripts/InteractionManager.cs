using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InteractionManager : MonoBehaviour
{
    [SerializeField] private AudioItem _coinSound;
    [SerializeField] private AudioItem _crashCarSound;
    [SerializeField] private GameObject _engineSound;

    [SerializeField] private ScoreManager _scoreManager;

    [SerializeField] private Scrollbar _healthScrollbar;

    [SerializeField] private TextMeshProUGUI _moneyText;
    [SerializeField] private TextMeshProUGUI _moneyTextDeath;
    [SerializeField] private TextMeshProUGUI _healthText;

    private int _health;
    [SerializeField] private int _money;
    private int _oldMoney;

    private CarControllerOld _carController;
    private FeaturesOfCar _featuresOfCar;
    [SerializeField] private SpawnManager _spawnManager;
    [SerializeField] private Traffic _traffic;
    [SerializeField] private LevelScript _levelScript;
    [SerializeField] private PlatformSpawner _platformSpawner;

    [SerializeField] private GameObject _deadMenu;
    [SerializeField] private GameObject _multiplyButton;
    [SerializeField] private GameObject _crashCarDead;
    [SerializeField] private GameObject _fuelDead;
    [SerializeField] private GameObject _ran;

    [SerializeField] private Image _damageAnimPanel;
    [SerializeField] private AnimationCurve _damageAnimCurve;
    [SerializeField] private float _durationOfAnim;

    [SerializeField] private bool _isRanActive;

    [SerializeField] private Animator _nitroBufAnim;

    private void Start()
    {
        GetComponent<Rigidbody>().sleepThreshold = 0;

        _carController = GetComponent<CarControllerOld>();
        _featuresOfCar = GetComponent<FeaturesOfCar>();

        if (PlayerPrefs.HasKey("Money") == false)
            _money = 10;
        else
            _money = PlayerPrefs.GetInt("Money");

        _moneyText.text = $"Денег: {_money}$";
        _isRanActive = _ran != null;

        _oldMoney = _money;
    }

    public void InitializeHealth(int health)
    {
        _health = health;
        _healthText.text = "Здоровья: " + _health;
        _healthScrollbar.size = 1;
    }

    private void GetDamage(int damage)
    {
        if (_isRanActive == false)
        {
            _healthScrollbar.size = _healthScrollbar.size >= damage * _healthScrollbar.size / _health ?
            _healthScrollbar.size - damage * _healthScrollbar.size / _health : 0;

            _health = _health >= damage ? _health - damage : 0;
            _healthText.text = "Здоровья: " + _health;

            StartCoroutine(DamageAnimation(_durationOfAnim > 0 ? _durationOfAnim : 1));
        }
        else
            DestroyRan();
    }

    private void DestroyRan()
    {
        Destroy(_ran);
        _isRanActive = false;
    }

    IEnumerator DamageAnimation(float duration)
    {
        float progress = 0.0f;
        Color32 color = _damageAnimPanel.color;

        while (progress < 1.0f)
        {
            progress += Time.deltaTime / duration;
            _damageAnimPanel.color = new Color32(color.r, color.g, color.b, (byte)(_damageAnimCurve.Evaluate(progress) * 255));

            yield return null;
        }
    }

    public void MultiplyMoney()
    {
        _money += _money - _oldMoney;
        _moneyTextDeath.text = $"Ты заработал: {_money - _oldMoney}$";

        PlayerPrefs.SetInt("Money", _money);
    }

    public void DeadMenu()
    {
        _scoreManager.SaveScore();
        _scoreManager.enabled = false;

        _deadMenu.SetActive(true);
        _carController.UIComponents.SetActive(false);

        if (_carController.IsFuel)
            _crashCarDead.SetActive(true);
        else
            _fuelDead.SetActive(true);

        _carController.enabled = false;
        _spawnManager.IsSpawnActive = false;
        _traffic.IsTrafficActive = false;
        _carController.enabled = false;
        _levelScript.enabled = false;
        _platformSpawner.enabled = false;

        Destroy(GetComponent<BoxCollider>());
        Destroy(GetComponent<Rigidbody>());
        Destroy(_engineSound);

        _moneyTextDeath.text = $"Ты заработал: {_money - _oldMoney}$";
        _multiplyButton.SetActive(_money - _oldMoney > 0);
    }

    private void OnCollisionEnter(Collision collision)
    {
        string tag = collision.gameObject.tag;

        if (tag == "Coin")
        {
            _coinSound.PlayAudio();

            _moneyText.text = $"Денег: {++_money}$";
            PlayerPrefs.SetInt("Money", _money);

            Destroy(collision.gameObject);
        }
        else if (tag == "Rock")
        {
            DestroyRan();
            GetDamage(_health);
        }
        else if (tag == "NPC")
        {
            collision.gameObject.GetComponent<NPCController>().DeadAnim(transform.position, _carController.Speed);
            _crashCarSound.PlayAudio();

            GetDamage(1);
        }
        else if (tag == "Obgon")
        {
            NPCController npc = collision.gameObject.GetComponentInParent<NPCController>();

            if ((npc.transform.rotation.y == 1 || npc.transform.rotation.y == 0) && _carController.Speed - npc.NpsLocalSpeed > 5 && _carController.CurrentNitro < 10 && npc.IsActive)
            {
                _carController.CurrentNitro += (_carController.CurrentNitro > 7.5f ? 10 - _carController.CurrentNitro : 2.5f);
                _nitroBufAnim.SetTrigger("Play");
            }
        }
        else if (tag == "Conus")
        {
            GetDamage(1);
            Destroy(collision.gameObject);
        }

        if (_health == 0)
        {
            _crashCarSound.PlayAudio();
            _featuresOfCar.PlayerDeadAnim(collision.gameObject.transform.position);

            DeadMenu();
        }
    }
}
