import React, { useState } from 'react';
import axios from 'axios';

const Contact = () => {
  const [formData, setFormData] = useState({
    name: '',
    email: '',
    message: ''
  });
  const [formMessage, setFormMessage] = useState('');
  const [isSubmitting, setIsSubmitting] = useState(false);

  const handleChange = (e) => {
    setFormData({
      ...formData,
      [e.target.name]: e.target.value
    });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setIsSubmitting(true);
    setFormMessage('Enviando mensagem...');

    try {
      await axios.post('/api/contact', formData, {
        headers: {
          'Content-Type': 'application/json'
        }
      });
      
      setFormMessage(`Obrigado, ${formData.name}! Sua mensagem foi enviada com sucesso. Entraremos em contato em breve!`);
      
      setFormData({
        name: '',
        email: '',
        message: ''
      });
      setTimeout(() => {
        setFormMessage('');
      }, 5000);
    } catch (error) {
      console.error('Erro ao enviar mensagem:', error);
      
      let errorMessage = 'Erro ao enviar mensagem. Por favor, tente novamente.';
      if (error.response) {
        errorMessage = error.response.data?.message || errorMessage;
      } else if (error.request) {
        errorMessage = 'Não foi possível conectar ao servidor. Verifique se a API está rodando.';
      }
      
      setFormMessage(errorMessage);
      setTimeout(() => {
        setFormMessage('');
      }, 5000);
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <section id="contato" className="section contact-section">
      <div className="container">
        <h2 className="section-title">Entre em Contato</h2>
        <p className="contact-description">
          Tem um projeto em mente? Entre em contato conosco e vamos transformar sua ideia em realidade!
        </p>
        <form className="contact-form" onSubmit={handleSubmit}>
          <div className="form-group">
            <label htmlFor="name">Nome</label>
            <input
              type="text"
              id="name"
              name="name"
              value={formData.name}
              onChange={handleChange}
              required
            />
          </div>
          <div className="form-group">
            <label htmlFor="email">E-mail</label>
            <input
              type="email"
              id="email"
              name="email"
              value={formData.email}
              onChange={handleChange}
              required
            />
          </div>
          <div className="form-group">
            <label htmlFor="message">Mensagem</label>
            <textarea
              id="message"
              name="message"
              rows="5"
              value={formData.message}
              onChange={handleChange}
              required
            />
          </div>
          <button
            type="submit"
            className="btn-primary"
            disabled={isSubmitting}
          >
            {isSubmitting ? 'Enviando...' : 'Enviar Mensagem'}
          </button>
          {formMessage && (
            <p className={`form-message ${formMessage.includes('sucesso') ? 'success' : formMessage.includes('Erro') ? 'error' : ''}`}>
              {formMessage}
            </p>
          )}
        </form>
      </div>
    </section>
  );
};

export default Contact;
