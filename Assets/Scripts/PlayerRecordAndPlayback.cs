using UnityEngine;
using System.Collections;

public class PlayerRecordAndPlayback : MonoBehaviour {

	public Transform playerTransform;
	private Queue recordedFrames;
	private float startTime;

	enum RecordingState {Idle, Record, Play};
	public class CapturedFrame
	{
		float timeSinceLast;
		Vector3 playerPosition;
		CapturedFrame(float timeSinceLast, Vector3 playerPosition)
		{
			this.timeSinceLast = timeSinceLast;
			this.playerPosition = playerPosition;
		}
	}
	

	void Awake()
	{
		this.recordedFrames = new Queue<CapturedFrame>();
	}

	void Update()
	{
		/* if we press the rec button
		 * 	if we're idle
		 * 	 start recording
		 *  if we're recording
		 *   start playing
		 *  if we're playing
		 *   stop playing
		 */

	}

	void CaptureFrame()
	{
		CapturedFrame newFrame = new CapturedFrame(Time.deltaTime,playerTransform.position);
		this.recordedFrames.Enqueue(newFrame);
	}

	void PlayBack()
	{

	}
}
