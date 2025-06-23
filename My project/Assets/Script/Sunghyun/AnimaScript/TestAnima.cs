using UnityEngine;

public class AnimaInventoryTestLoader : MonoBehaviour
{
    private void Start()
    {
        var testAnima = ScriptableObject.CreateInstance<AnimaDataSO>();
        testAnima.Initialize("test1");
        AnimaInventoryManager.Instance.AddAnima(testAnima);
        var testAnima1 = ScriptableObject.CreateInstance<AnimaDataSO>();
        testAnima1.Initialize("test2");
        AnimaInventoryManager.Instance.AddAnima(testAnima1);
        var testAnima2 = ScriptableObject.CreateInstance<AnimaDataSO>();
        testAnima2.Initialize("saejin");
        AnimaInventoryManager.Instance.AddAnima(testAnima2);
        var testAnima3 = ScriptableObject.CreateInstance<AnimaDataSO>();
        testAnima3.Initialize("saejin2");
        AnimaInventoryManager.Instance.AddAnima(testAnima3);
        var testAnima4 = ScriptableObject.CreateInstance<AnimaDataSO>();
        testAnima4.Initialize("test1");
        AnimaInventoryManager.Instance.AddAnima(testAnima4);
        var testAnima5 = ScriptableObject.CreateInstance<AnimaDataSO>();
        testAnima5.Initialize("test2");
        AnimaInventoryManager.Instance.AddAnima(testAnima5);
        var testAnima6 = ScriptableObject.CreateInstance<AnimaDataSO>();
        testAnima6.Initialize("saejin");
        AnimaInventoryManager.Instance.AddAnima(testAnima6);
        var testAnima7 = ScriptableObject.CreateInstance<AnimaDataSO>();
        testAnima7.Initialize("saejin2");
        AnimaInventoryManager.Instance.AddAnima(testAnima7);
        var testAnima8 = ScriptableObject.CreateInstance<AnimaDataSO>();
        testAnima8.Initialize("test1");
        AnimaInventoryManager.Instance.AddAnima(testAnima8);
        var testAnima9 = ScriptableObject.CreateInstance<AnimaDataSO>();
        testAnima9.Initialize("test2");
        AnimaInventoryManager.Instance.AddAnima(testAnima9);
        var testAnima10 = ScriptableObject.CreateInstance<AnimaDataSO>();
        testAnima10.Initialize("saejin");
        AnimaInventoryManager.Instance.AddAnima(testAnima10);
        var testAnima11 = ScriptableObject.CreateInstance<AnimaDataSO>();
        testAnima11.Initialize("saejin2");
        AnimaInventoryManager.Instance.AddAnima(testAnima11);
        var testAnima12 = ScriptableObject.CreateInstance<AnimaDataSO>();
        testAnima12.Initialize("test1");
        AnimaInventoryManager.Instance.AddAnima(testAnima12);
        var testAnima13 = ScriptableObject.CreateInstance<AnimaDataSO>();
        testAnima13.Initialize("test2");
        AnimaInventoryManager.Instance.AddAnima(testAnima13);
        var testAnima14 = ScriptableObject.CreateInstance<AnimaDataSO>();
        testAnima14.Initialize("saejin");
        AnimaInventoryManager.Instance.AddAnima(testAnima14);
    }
}
