using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlight : MonoBehaviour
{
    [SerializeField] private List<Renderer> renderers;
    [SerializeField] private Color color = Color.white;

    private List<Material> materials;

    //renderer ��������
    private void Awake()
    {
        //�ڽĿ�����Ʈ�� ���� ���� material�� ���� �� �ֱ⶧���� "s" �Ҿ���� materials
        materials = new List<Material>();
        foreach(var renderer in renderers)
        {
            materials.AddRange(new List<Material>(renderer.materials));
        }
    }

    public void ToggleHighlight(bool val)
    {
        if(val)
        {
            foreach(var material in materials)
            {
                //_EMISSION Ȱ��ȭ�ؾ���
                material.EnableKeyword("_EMISSION");
                //���� ���� ����
                material.SetColor("_EmissionColor", color);
            }
        }
        else
        {
            foreach(var material in materials)
            {
                //_EMISSION ��Ȱ��ȭ
                //�ٸ������� _EMISSION������ ������� �ʴ� ���
                material.DisableKeyword("_EMISSION");
            }
        }
    }
}
