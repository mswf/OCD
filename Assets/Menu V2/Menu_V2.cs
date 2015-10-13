/*  _________________________________________________
    A new version of the Oculus Drift menu. A lot of
    code was revised from old Menu.cs.
    _________________________________________________ */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Menu_V2 : MonoBehaviour
{

	#region Definition list
	
	public enum Page
	{
		Main,
		Settings,
		Profiles,
		ProfileCreation,
		Leaderboard,
		Graphics,
		Audio,
		Controls,
		Calibration,
		Credits
	}
	public Page startingPage;
	public bool lockCursor;
	
	private Page _currentPage;
	private GameObject _page;
	private IPage _pageComponent;
	
	private MenuControls controller;
	public Profile profile = new Profile();
	
	[HideInInspector]
	public MenuSounds audioSource;
	
	#endregion
	
	
	private void Awake()
	{
		Time.timeScale = 1;
	}
	
	private void Start()
	{
		//Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = lockCursor;
		//Screen.lockCursor = lockCursor;
		audioSource = GetComponent<MenuSounds>();
		
		if (profile.getProfileID() < 1)
		{
			_currentPage = Page.Profiles;
		}
		
		else
		{
			_currentPage = startingPage;
		}
		
		WeikiesRiftHack.ResetOrientation();
	}
	
	private IEnumerator ChangePage(Page newPage)
	{
		if (_pageComponent.animator != null)
		{
			yield return new WaitForSeconds(_pageComponent.animator.time);
		}
		
		_currentPage = newPage;
		Destroy(_page);
		StopCoroutine("ChangePage");
	}
	
	private void Update()
	{
		// Set current page
		switchPageTo(_currentPage.ToString());
	}
	
	private void switchPageTo(string path)
	{
		if (_page == null)
		{
			// Create new page, make child of this object and get reference of the component
			_page = Instantiate(Resources.Load("Menu/Pages/" + path + "Page"), new Vector3(), new Quaternion()) as GameObject;
			_page.transform.parent = gameObject.transform;
			_page.transform.localPosition = new Vector3();
			//_page.transform.localRotation = new Quaternion();
			_pageComponent = _page.GetComponent<IPage>();
			
			// Have to set reference to this script to get accese in changing pages
			_pageComponent.SetReference(GetComponent<Menu_V2>(), GetComponent<MenuControls>());
		}
	}
	
	public bool ChangeCurrentPageTo(Page newPage)
	{
	
		if (_currentPage != newPage)
		{
			StartCoroutine(ChangePage(newPage));
			return true;
		}
		
		else
		{
			return false;
		}
	}
	
}
