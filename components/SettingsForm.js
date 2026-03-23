import { useEffect, useState } from 'react';

const LANGUAGES = [
  { label: 'English', value: 'en' },
  { label: 'German', value: 'de' },
  { label: 'Spanish', value: 'es' },
  { label: 'French', value: 'fr' },
];

const THEMES = [
  { label: 'System', value: 'system' },
  { label: 'Light', value: 'light' },
  { label: 'Dark', value: 'dark' },
];

export default function SettingsForm({ initialSettings, onSubmit, helperText, isSaving }) {
  const [formState, setFormState] = useState(initialSettings);
  const [errors, setErrors] = useState({});

  useEffect(() => {
    setFormState(initialSettings);
  }, [initialSettings]);

  const validate = (state) => {
    const nextErrors = {};
    if (Number.isNaN(Number(state.autoSaveInterval))) {
      nextErrors.autoSaveInterval = 'Auto save interval must be a number.';
    } else if (state.autoSaveInterval < 1 || state.autoSaveInterval > 60) {
      nextErrors.autoSaveInterval = 'Interval must be between 1 and 60 minutes.';
    }
    if (!state.language) {
      nextErrors.language = 'Please select a language.';
    }
    if (!state.theme) {
      nextErrors.theme = 'Please select a theme.';
    }
    setErrors(nextErrors);
    return Object.keys(nextErrors).length === 0;
  };

  const handleChange = (event) => {
    const { name, type, value, checked } = event.target;
    setFormState((prev) => ({
      ...prev,
      [name]: type === 'checkbox' ? checked : type === 'number' ? Number(value) : value,
    }));
  };

  const handleSubmit = (event) => {
    event.preventDefault();
    if (validate(formState)) {
      onSubmit(formState);
    }
  };

  return (
    <form onSubmit={handleSubmit} style={{ display: 'grid', gap: '1.5rem' }}>
      <label style={{ display: 'grid', gap: '.5rem' }}>
        Theme
        <select name="theme" value={formState.theme} onChange={handleChange}>
          {THEMES.map((theme) => (
            <option key={theme.value} value={theme.value}>
              {theme.label}
            </option>
          ))}
        </select>
        {errors.theme && <span style={{ color: '#a30000' }}>{errors.theme}</span>}
      </label>

      <label style={{ display: 'flex', alignItems: 'center', gap: '.5rem' }}>
        <input
          type="checkbox"
          name="notifications"
          checked={formState.notifications}
          onChange={handleChange}
        />
        Enable notifications
      </label>

      <label style={{ display: 'grid', gap: '.5rem' }}>
        Auto save interval (minutes)
        <input
          type="number"
          min={1}
          max={60}
          name="autoSaveInterval"
          value={formState.autoSaveInterval}
          onChange={handleChange}
        />
        {helperText && !errors.autoSaveInterval && (
          <span style={{ color: '#555', fontSize: '.9rem' }}>{helperText}</span>
        )}
        {errors.autoSaveInterval && <span style={{ color: '#a30000' }}>{errors.autoSaveInterval}</span>}
      </label>

      <label style={{ display: 'grid', gap: '.5rem' }}>
        Language
        <select name="language" value={formState.language} onChange={handleChange}>
          <option value="">Select language</option>
          {LANGUAGES.map((language) => (
            <option key={language.value} value={language.value}>
              {language.label}
            </option>
          ))}
        </select>
        {errors.language && <span style={{ color: '#a30000' }}>{errors.language}</span>}
      </label>

      <button
        type="submit"
        style={{
          padding: '.75rem 1.25rem',
          backgroundColor: '#111',
          color: '#fff',
          border: 'none',
          cursor: 'pointer',
        }}
      >
        {isSaving ? 'Saved' : 'Save settings'}
      </button>
    </form>
  );
}