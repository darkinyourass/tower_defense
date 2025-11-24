using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

/// <summary>
/// UI для отображения времени до следующей волны с круговым прогресс-баром
/// </summary>
public class WaveTimerUI : MonoBehaviour
{
	[Header("UI References")]
	[SerializeField] private GameObject waveTimerPanel;
	[SerializeField] private Image circleProgressFill;
	[SerializeField] private TextMeshProUGUI timerText;
	[SerializeField] private Button startWaveButton;
	[SerializeField] private TextMeshProUGUI bonusText;

	[Header("Visual Settings")]
	[SerializeField] private Gradient progressColorGradient;
	[SerializeField] private bool useColorGradient = true;

	// Цвета для разных стадий (если не используем градиент)
	[SerializeField] private Color colorStart = new Color(0.3f, 0.8f, 0.3f); // Зелёный
	[SerializeField] private Color colorMiddle = new Color(1f, 0.9f, 0.2f); // Жёлтый
	[SerializeField] private Color colorEnd = new Color(0.9f, 0.2f, 0.2f); // Красный

	[Header("Animation")]
	[SerializeField] private bool animateScale = true;
	[SerializeField] private float pulseSpeed = 2f;
	[SerializeField] private float pulseAmount = 0.05f;

	private Vector3 _originalScale;
	private int _lastBonus = 0;

	private void OnEnable()
	{
		Debug.Log("WaveTimerUI: OnEnable вызван, подписываемся на события");
		Spawner.OnWaveCooldownChanged += HandleWaveCooldownChanged;
	}

	private void OnDisable()
	{
		Spawner.OnWaveCooldownChanged -= HandleWaveCooldownChanged;
	}

	private void Start()
	{
		// Подключаем кнопку
		startWaveButton.onClick.AddListener(OnStartWaveButtonClicked);

		// Сохраняем оригинальный размер для анимации
		if (circleProgressFill != null)
		{
			_originalScale = circleProgressFill.transform.localScale;
		}

		// Изначально скрываем панель
		waveTimerPanel.SetActive(false);

		// Настраиваем градиент если не настроен
		SetupDefaultGradient();
	}

	private void Update()
	{
		if (Spawner.Instance == null) return;

		if (waveTimerPanel.activeSelf && Spawner.Instance != null)
		{
			UpdateProgressBar();
			UpdateTimerText();
			UpdateBonusText();

			if (animateScale)
			{
				AnimatePulse();
			}
		}
	}

	/// <summary>
	/// Обновляет прогресс-бар
	/// </summary>
	private void UpdateProgressBar()
	{
		float progress = 1f - Spawner.Instance.WaveCooldownProgress; // Инвертируем (заполнение, а не опустошение)

		// Плавное заполнение
		circleProgressFill.fillAmount = Mathf.Lerp(
			circleProgressFill.fillAmount,
			progress,
			Time.deltaTime * 10f
		);

		// Меняем цвет в зависимости от прогресса
		UpdateProgressColor(progress);
	}

	/// <summary>
	/// Обновляет цвет прогресс-бара
	/// </summary>
	private void UpdateProgressColor(float progress)
	{
		if (useColorGradient && progressColorGradient != null)
		{
			// Используем градиент
			circleProgressFill.color = progressColorGradient.Evaluate(progress);
		}
		else
		{
			// Используем три цвета с плавными переходами
			Color currentColor;

			if (progress < 0.5f)
			{
				// От начала (зелёный) к середине (жёлтый)
				currentColor = Color.Lerp(colorStart, colorMiddle, progress * 2f);
			}
			else
			{
				// От середины (жёлтый) к концу (красный)
				currentColor = Color.Lerp(colorMiddle, colorEnd, (progress - 0.5f) * 2f);
			}

			circleProgressFill.color = currentColor;
		}
	}

	/// <summary>
	/// Обновляет текст с оставшимися секундами
	/// </summary>
	private void UpdateTimerText()
	{
		float timeRemaining = Spawner.Instance.WaveCooldownRemaining;
		int seconds = Mathf.CeilToInt(timeRemaining);

		timerText.text = seconds.ToString();

		// Опционально: меняем цвет текста
		if (seconds <= 3)
		{
			timerText.color = colorEnd; // Красный
		}
		else if (seconds <= 5)
		{
			timerText.color = colorMiddle; // Жёлтый
		}
		else
		{
			timerText.color = Color.white;
		}
	}

	/// <summary>
	/// Обновляет текст бонуса на кнопке
	/// </summary>
	private void UpdateBonusText()
	{
		if (Spawner.Instance == null) return;

		float progress = Spawner.Instance.WaveCooldownProgress;
		int bonus = Mathf.RoundToInt(progress * 25); // 25 - это _earlyStartBonus из Spawner

		// Обновляем только если изменился (оптимизация)
		if (bonus != _lastBonus)
		{
			_lastBonus = bonus;
			bonusText.text = bonus > 0 ? $"+{bonus} 💰" : "No bonus";

			// Меняем прозрачность если бонус 0
			Color bonusColor = bonusText.color;
			bonusColor.a = bonus > 0 ? 1f : 0.5f;
			bonusText.color = bonusColor;
		}
	}

	/// <summary>
	/// Анимация пульсации круга
	/// </summary>
	private void AnimatePulse()
	{
		float scale = 1f + Mathf.Sin(Time.time * pulseSpeed) * pulseAmount;
		circleProgressFill.transform.localScale = _originalScale * scale;
	}

	/// <summary>
	/// Показывает/скрывает панель при смене статуса кулдауна
	/// </summary>
	private void HandleWaveCooldownChanged(bool isCooldownActive)
	{
		Debug.Log($"WaveTimerUI: Получено событие cooldown = {isCooldownActive}");
		waveTimerPanel.SetActive(isCooldownActive);

		if (isCooldownActive)
		{
			Debug.Log("WaveTimerUI: Панель должна появиться!");
			// Сбрасываем прогресс при появлении
			circleProgressFill.fillAmount = 0f;
			_lastBonus = 0;
		}
	}

	/// <summary>
	/// Обработчик клика на кнопку
	/// </summary>
	private void OnStartWaveButtonClicked()
	{
		if (Spawner.Instance != null && Spawner.Instance.IsBetweenWaves)
		{
			Spawner.Instance.SkipWaveCooldown();
		}
	}

	/// <summary>
	/// Настраивает градиент по умолчанию если не задан
	/// </summary>
	private void SetupDefaultGradient()
	{
		if (progressColorGradient == null)
		{
			progressColorGradient = new Gradient();

			GradientColorKey[] colorKeys = new GradientColorKey[3];
			colorKeys[0] = new GradientColorKey(colorStart, 0f);
			colorKeys[1] = new GradientColorKey(colorMiddle, 0.5f);
			colorKeys[2] = new GradientColorKey(colorEnd, 1f);

			GradientAlphaKey[] alphaKeys = new GradientAlphaKey[2];
			alphaKeys[0] = new GradientAlphaKey(1f, 0f);
			alphaKeys[1] = new GradientAlphaKey(1f, 1f);

			progressColorGradient.SetKeys(colorKeys, alphaKeys);
		}
	}
}
