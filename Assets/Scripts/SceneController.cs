using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class SceneController : MonoBehaviour
{
	public const int GridRows = 2;
	public const int GridCols = 4;
	public const float OffsetX = 2f;
	public const float OffsetY = 2.5f;
	public bool CanReveal
	{
		get { return _secondReveal == null; }
	}

	private MemoryCard _firstReveal;
	private MemoryCard _secondReveal;
	private int _score = 0;

	[SerializeField] private MemoryCard originalCard;
	[SerializeField] private Sprite[] images;
	[SerializeField] private TextMesh scoreLabel;

	void Start()
	{
		Vector3 startPosition = originalCard.transform.position;

		int[] numbers = { 0, 0, 1, 1, 2, 2, 3, 3 };

		numbers = ShuffleArray(numbers);
		

		for(int i = 0; i < GridCols; i++)
		{
			for(int j = 0; j < GridRows; j++)
			{
				MemoryCard card;
				if (i == 0 && j == 0)
				{
					card = originalCard;
				}
				else
				{
					card = Instantiate(originalCard);
				}
				int index = j * GridCols + i;
				int id = numbers[index];
				card.SetCard(id, images[id]);

				float posX = (OffsetX * i) + startPosition.x;
				float posY = -(OffsetY * j) + startPosition.y;
				card.transform.position = new Vector3(posX, posY, startPosition.z);
			}
		}
	}

	public int[] ShuffleArray(int[] numbers)
	{
		int[] newArray = numbers.Clone() as int[];

		for(int i = 0; i < newArray.Length; i++)
		{
			int tmp = newArray[i];
			int r = Random.Range(i, newArray.Length);
			newArray[i] = newArray[r];
			newArray[r] = tmp;
		}
		return newArray;
	}

	public void CardReveal(MemoryCard card)
	{
		if(_firstReveal == null)
		{
			_firstReveal = card;
		}
		else
		{
			_secondReveal = card;
			StartCoroutine(CheckMatch());
		}
	}

	public IEnumerator CheckMatch()
	{
		if(_firstReveal.Id == _secondReveal.Id)
		{
			_score++;
			scoreLabel.text = "Score: " + _score;
		}
		else
		{
			yield return new WaitForSeconds(.5f);

			_firstReveal.Unreveal();
			_secondReveal.Unreveal();
		}

		_firstReveal = null;
		_secondReveal = null;
	}

	public void Restart()
	{
		Application.LoadLevel("SampleScene");
	}
}
