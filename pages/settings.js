import { useCallback, useMemo, useState } from 'react';
import SettingsForm from '../components/SettingsForm';

const DEFAULT_SETTINGS = {
  theme: 'light',
  notifications: true,
  autoSaveInterval: 5,
  language: 'en',
};

export default function SettingsPage() {
  const [settings, setSettings] = useState(DEFAULT_SETTINGS);
  const [status, setStatus] = useState({ type: null, message: '' });

  const handleSave = useCallback(async (updatedSettings) => {
    try {
      setSettings(updatedSettings);
      setStatus({ type: 'success', message: 'Settings saved successfully.' });
    } catch (error) {
      setStatus({ type: 'error', message: 'Unable to save settings. Please try again.' });
    } finally {
      setTimeout(() => setStatus({ type: null, message: '' }), 4000);
    }
  }, []);

  const helperText = useMemo(() => {
    if (settings.autoSaveInterval <= 2) {
      return 'Auto save is very frequent.';
    }
    if (settings.autoSaveInterval >= 30) {
      return 'Auto save is infrequent. Consider lowering the interval.';
    }
    return '';
  }, [settings.autoSaveInterval]);

  return (
    <main style={{ padding: '2rem 1rem', maxWidth: 720, margin: '0 auto' }}>
      <h1 style={{ marginBottom: '1rem' }}>Application Settings</h1>
      <p style={{ marginBottom: '2rem', color: '#555' }}>
        Configure how the application behaves. These preferences are stored locally for this session.
      </p>
      <SettingsForm
        initialSettings={settings}
        onSubmit={handleSave}
        helperText={helperText}
        isSaving={status.type === 'success'}
      />
      {status.type && (
        <p
          style={{
            marginTop: '1.5rem',
            color: status.type === 'success' ? '#0a7a07' : '#a30000',
          }}
        >
          {status.message}
        </p>
      )}
    </main>
  );
}