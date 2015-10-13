using UnityEngine;
using System.Collections;

namespace OculusMenu
{
	public class Label : MonoBehaviour 
	{
		protected MenuObject _type;
		public MenuObject ObjectType
		{
			get { return _type; }
		}

		public GameObject Image;
		public GameObject Text;

		private TextMesh _text;

		protected virtual void Awake()
		{
			_type = MenuObject.Label;
			_text = Text.GetComponent<TextMesh>();
		}

		public string GetText()
		{
			return _text.text;
		}

		public void SetText(string text)
		{
			_text.text = text;
		}

		public void SetTextColor(Color color)
		{
			_text.color = color;
		}

		public Color GetTextColor()
		{
			return _text.color;
		}

		public void SetCharacterSize(float size)
		{
			_text.characterSize = size;
		}

		/* //OLD CODE
		protected void InitializeText()
		{
			if (_text != null) 
			{  

				Vector3 pos = this.transform.position;
				_text = Instantiate(textMesh, new Vector3(pos.x, pos.y, pos.z), new Quaternion()) as GameObject;
				_text.transform.parent = gameObject.transform;
				_component = _text.GetComponent<TextMesh>();

			}
		}

		private void InitializeImage()
		{
			if (_image != null){}
				
				Vector3 pos = this.transform.position;
				_image = Instantiate(Image, new Vector3(pos.x, pos.y, pos.z), new Quaternion()) as GameObject;
				_image.transform.parent = gameObject.transform;

		}

		public virtual void SetPosition(Vector3 position)
		{
			if(_text != null) _text.transform.position = position;
			if(_image != null) _image.transform.position = position;
		}

		public virtual void SetRotation(Quaternion rotation)
		{
			if(_text != null) _text.transform.rotation = rotation;
			if(_image != null) _image.transform.rotation = rotation;
		}
		*/


	}
}
