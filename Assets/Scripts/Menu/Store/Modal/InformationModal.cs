using System;
using UnityEngine;
using UnityEngine.UI;

namespace Menu.Store.Modal
{
    public class InformationModal : MonoBehaviour
    {
        [SerializeField] private Button closeButton;

        private Animator _animator;

        private void Awake()
        {
            if (closeButton != null)
                closeButton.onClick.AddListener(CloseModal);

            _animator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            _animator.Play("Show");
        }

        private void CloseModal()
        {
            gameObject.SetActive(false);
        }
    }
}