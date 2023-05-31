using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;


public class AttackEffects : MonoBehaviour
{
	public Renderer groundRenderer;
	public Collider groundCollider;
	[Space]
	[Space]
	public Text EffectLabel;
	public Text EffectIndexLabel;
	public int particleNum;
	//-------------------------------------------------------------

	private GameObject[] ParticleExamples;
	private int exampleIndex;
	private bool slowMo;
	private Vector3 defaultCamPosition;
	private Quaternion defaultCamRotation;
	
	private List<GameObject> onScreenParticles = new List<GameObject>();
	
	//-------------------------------------------------------------
	
	void Awake()
	{
		List<GameObject> particleExampleList = new List<GameObject>();
		int nbChild = this.transform.childCount;
		for(int i = 0; i < nbChild; i++)
		{
			GameObject child = this.transform.GetChild(i).gameObject;
			particleExampleList.Add(child);
		}
		particleExampleList.Sort( delegate(GameObject o1, GameObject o2) { return o1.name.CompareTo(o2.name); } );
		ParticleExamples = particleExampleList.ToArray();
		
		
		StartCoroutine("CheckForDeletedParticles");
		
	}
	
	void Update()
	{
		
		//if(Input.GetMouseButtonDown(0))
		//{
		//	RaycastHit hit = new RaycastHit();
		//	if(groundCollider.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 9999f))
		//	{
		//		GameObject particle = spawnParticle();
		//		particle.transform.position = hit.point + particle.transform.position;
		//	}
		//}
		
	}

	//-------------------------------------------------------------
	// MESSAGES


	
	//-------------------------------------------------------------
	// SYSTEM
	public void playEffect(int effectNumber, Vector3 effectPosition)
	{
		particleNum = effectNumber;
        GameObject particle = spawnParticle();
        particle.transform.position = effectPosition;
    }
	private GameObject spawnParticle()
	{
		GameObject particles = (GameObject)Instantiate(ParticleExamples[particleNum]);
		particles.transform.position = new Vector3(0,particles.transform.position.y,0);
		#if UNITY_3_5
			particles.SetActiveRecursively(true);
		#else
			particles.SetActive(true);
//			for(int i = 0; i < particles.transform.childCount; i++)
//				particles.transform.GetChild(i).gameObject.SetActive(true);
		#endif
		
		ParticleSystem ps = particles.GetComponent<ParticleSystem>();

#if UNITY_5_5_OR_NEWER
		if (ps != null)
		{
			var main = ps.main;
			if (main.loop)
			{
				ps.gameObject.AddComponent<CFX_AutoStopLoopedEffect>();
				ps.gameObject.AddComponent<CFX_AutoDestructShuriken>();
			}
		}
#else
		if(ps != null && ps.loop)
		{
			ps.gameObject.AddComponent<CFX_AutoStopLoopedEffect>();
			ps.gameObject.AddComponent<CFX_AutoDestructShuriken>();
		}
#endif

		onScreenParticles.Add(particles);
		
		return particles;
	}
	
	IEnumerator CheckForDeletedParticles()
	{
		while(true)
		{
			yield return new WaitForSeconds(5.0f);
			for(int i = onScreenParticles.Count - 1; i >= 0; i--)
			{
				if(onScreenParticles[i] == null)
				{
					onScreenParticles.RemoveAt(i);
				}
			}
		}
	}
	
	private void destroyParticles()
	{
		for(int i = onScreenParticles.Count - 1; i >= 0; i--)
		{
			if(onScreenParticles[i] != null)
			{
				GameObject.Destroy(onScreenParticles[i]);
			}
			
			onScreenParticles.RemoveAt(i);
		}
	}
}
