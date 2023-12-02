using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wreckage : MonoBehaviour
{
    public bool finishDrop = false;

    public void ResetPos()
    {
        //잔해물 자기 자리로
        finishDrop = false;
        transform.position = new Vector3(transform.position.x, 50, transform.localPosition.z);
    }

    IEnumerator Drop_Wreckage(Vector3 wreckageRandomPos)
    {
        //떨어뜨림.
        Effect effect = GameManager.Instance.objectPooling.ShowEffect("Wreckage_Warning01");
        effect.transform.position = wreckageRandomPos;

        yield return new WaitForSeconds(3f);
        float time = 0;
        float speed = 50;

        Vector3 targetPos = wreckageRandomPos;

        while (time < 5f)
        {
            time += Time.deltaTime;
            speed = Mathf.Lerp(50, 70, Time.time);
            transform.Translate(-Vector3.up * speed * Time.deltaTime);
            if (transform.localPosition.y <= wreckageRandomPos.y)
            {
                effect.StopEffect();
                float distance = Vector3.Distance(GameManager.instance.gameData.GetPlayerTransform().position, transform.position);
                if (distance < 18)
                    GameManager.Instance.cameraShake.ShakeCamera(1f, 2, 2);

                Effect smokeEffect = GameManager.Instance.objectPooling.ShowEffect("Smoke_Effect_01");
                Vector3 effectPos = transform.position;
                effectPos.y -= 1f;
                smokeEffect.transform.position = effectPos;
                finishDrop = true;
                break;
            }

            yield return null;
        }

        //쿵하는 이펙트
        //몇초후 

        yield return null;
    }

    public void StartDropWreckage(Vector3 wreckageRandomPos)
    {
        float randomY = UnityEngine.Random.Range(60f, 100f);
        Vector3 wreckagePos = new Vector3(wreckageRandomPos.x, randomY, wreckageRandomPos.z);
        transform.localPosition = wreckagePos;

        StartCoroutine(Drop_Wreckage(wreckageRandomPos));

    }


}

