import React from 'react';

const Hero = () => {
  const handleScrollToServices = (e) => {
    e.preventDefault();
    const section = document.getElementById('servicos');
    if (section) {
      const navHeight = document.querySelector('.navbar').offsetHeight;
      const targetPosition = section.offsetTop - navHeight;

      window.scrollTo({
        top: targetPosition,
        behavior: 'smooth'
      });
    }
  };

  return (
    <section id="inicio" className="hero">
      <div className="hero-content">
        <h1 className="hero-title">LiSoft</h1>
        <p className="hero-subtitle">Soluções em Sistemas para o seu Negócio</p>
        <p className="hero-description">
          Desenvolvemos sistemas robustos e confiáveis, desde integrações de pagamento até aplicativos e sites personalizados.
        </p>
        <a href="#servicos" className="btn-primary" onClick={handleScrollToServices}>
          Conheça Nossos Serviços
        </a>
      </div>
    </section>
  );
};

export default Hero;
