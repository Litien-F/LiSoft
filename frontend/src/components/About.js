import React, { useEffect, useRef } from 'react';

const About = () => {
  const aboutRef = useRef(null);

  useEffect(() => {
    const observer = new IntersectionObserver(
      (entries) => {
        entries.forEach(entry => {
          if (entry.isIntersecting) {
            entry.target.style.opacity = '1';
            entry.target.style.transform = 'translateY(0)';
          }
        });
      },
      { threshold: 0.1, rootMargin: '0px 0px -50px 0px' }
    );

    if (aboutRef.current) {
      aboutRef.current.style.opacity = '0';
      aboutRef.current.style.transform = 'translateY(30px)';
      aboutRef.current.style.transition = 'opacity 0.6s ease, transform 0.6s ease';
      observer.observe(aboutRef.current);
    }

    return () => {
      if (aboutRef.current) {
        observer.unobserve(aboutRef.current);
      }
    };
  }, []);

  return (
    <section id="sobre" className="section">
      <div className="container">
        <h2 className="section-title">Sobre a LiSoft</h2>
        <div className="about-content" ref={aboutRef}>
          <p className="about-text">
            A <strong>LiSoft</strong> é uma empresa especializada em desenvolvimento de sistemas completos,
            oferecendo soluções tecnológicas que atendem desde necessidades complexas até projetos mais simples.
          </p>
          <p className="about-text">
            Nossa expertise abrange desde <strong>sistemas de pagamento</strong> com integrações seguras
            aos principais gateways de pagamento do mercado, até o desenvolvimento de <strong>aplicativos</strong>
            e <strong>sites</strong> modernos e funcionais.
          </p>
          <p className="about-text">
            Trabalhamos com tecnologias confiáveis e seguimos as melhores práticas do mercado para garantir
            que nossos clientes recebam soluções de alta qualidade, seguras e escaláveis.
          </p>
        </div>
      </div>
    </section>
  );
};

export default About;
