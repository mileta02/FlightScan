import styles from './ConfirmModal.module.css'

export default function ConfirmModal({ title, message, confirmLabel = 'Potvrdi', onConfirm, onClose }) {
  if (!onConfirm) return null

  return (
    <div className={styles.overlay} onClick={onClose}>
      <div className={styles.modal} onClick={(e) => e.stopPropagation()}>
        <p className={styles.title}>{title}</p>
        {message && <p className={styles.message}>{message}</p>}
        <div className={styles.actions}>
          <button className={styles.cancelBtn} onClick={onClose}>Odustani</button>
          <button className={styles.confirmBtn} onClick={onConfirm}>{confirmLabel}</button>
        </div>
      </div>
    </div>
  )
}
