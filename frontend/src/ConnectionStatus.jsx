const ConnectionStatus = ({connectionError}) => {
  return (
    <div className={connectionError ? 'modal is-active' : 'modal'}>
      <div className="modal-background"></div>
      <div className="modal-content">
        <span className='is-size-3 has-text-danger'>Cannot connect to server :-|</span>
      </div>
    </div>
  );
}

export default ConnectionStatus;