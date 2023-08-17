using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Localization;

public class LanguageSelection : MonoBehaviour
{
    TMP_Dropdown m_Dropdownlanguage;
    AsyncOperationHandle m_InitializeOperation;
    private void Start()
    {
        m_Dropdownlanguage = GetComponent<TMP_Dropdown>();
        m_Dropdownlanguage.onValueChanged.AddListener(OnSelectionChanged);

        m_Dropdownlanguage.ClearOptions();
        m_Dropdownlanguage.options.Add(new TMP_Dropdown.OptionData("Loading..."));
        m_Dropdownlanguage.interactable = false;

        m_InitializeOperation = LocalizationSettings.SelectedLocaleAsync;
        if (m_InitializeOperation.IsDone)
        {
            InitializeCompleted(m_InitializeOperation);
        }
        else
        {
            m_InitializeOperation.Completed += InitializeCompleted;
        }
    }
    void InitializeCompleted(AsyncOperationHandle obj)
    {
        var options = new List<string>();
        int selectedOption = 0;
        var locales = LocalizationSettings.AvailableLocales.Locales;
        for (int i = 0; i < locales.Count; ++i)
        {
            var locale = locales[i];
            if (LocalizationSettings.SelectedLocale == locale)
                selectedOption = i;

            var displayName = locales[i].Identifier.CultureInfo != null ? locales[i].Identifier.CultureInfo.NativeName : locales[i].ToString();
            options.Add(displayName);
        }

        if (options.Count == 0)
        {
            options.Add("No Locales Available");
            m_Dropdownlanguage.interactable = false;
        }
        else
        {
            m_Dropdownlanguage.interactable = true;
        }

        m_Dropdownlanguage.ClearOptions();
        m_Dropdownlanguage.AddOptions(options);
        m_Dropdownlanguage.SetValueWithoutNotify(selectedOption);

        LocalizationSettings.SelectedLocaleChanged += LocalizationSettings_SelectedLocaleChanged;
    }
    void OnSelectionChanged(int index)
    {
        // Unsubscribe from SelectedLocaleChanged so we don't get an unnecessary callback from the change we are about to make.
        LocalizationSettings.SelectedLocaleChanged -= LocalizationSettings_SelectedLocaleChanged;

        var locale = LocalizationSettings.AvailableLocales.Locales[index];
        LocalizationSettings.SelectedLocale = locale;

        // Resubscribe to SelectedLocaleChanged so that we can stay in sync with changes that may be made by other scripts.
        LocalizationSettings.SelectedLocaleChanged += LocalizationSettings_SelectedLocaleChanged;
    }

    void LocalizationSettings_SelectedLocaleChanged(Locale locale)
    {
        // We need to update the dropdown selection to match.
        var selectedIndex = LocalizationSettings.AvailableLocales.Locales.IndexOf(locale);
        m_Dropdownlanguage.SetValueWithoutNotify(selectedIndex);
    }
}
