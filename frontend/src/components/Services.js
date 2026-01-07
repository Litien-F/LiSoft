import React, { useEffect, useRef } from 'react';

const Services = () => {
  const serviceCardsRef = useRef([]);

  useEffect(() => {
    const observer = new IntersectionObserver(
      (entries) => {
        entries.forEach(entry => {
          if (entry.isIntersecting) {
            entry.target.classList.add('visible');
          }
        });
      },
      { threshold: 0.1, rootMargin: '0px 0px -50px 0px' }
    );

    serviceCardsRef.current.forEach(card => {
      if (card) {
        observer.observe(card);
      }
    });

    return () => {
      serviceCardsRef.current.forEach(card => {
        if (card) {
          observer.unobserve(card);
        }
      });
    };
  }, []);

  const services = [
    {
      icon: '💳',
      title: 'Sistemas de Pagamento',
      description: 'Desenvolvemos sistemas de pagamento robustos com integrações aos principais gateways de pagamento do mercado. Soluções seguras, confiáveis e totalmente integradas ao seu negócio.',
      features: [
        'Integração com gateways confiáveis',
        'Segurança e conformidade',
        'Processamento rápido e eficiente'
      ]
    },
    {
      icon: '📱',
      title: 'Aplicativos',
      description: 'Criamos aplicativos funcionais e intuitivos para diversas plataformas. Desde apps simples até soluções mais complexas, sempre focados na experiência do usuário.',
      features: [
        'Design moderno e intuitivo',
        'Performance otimizada',
        'Compatibilidade multiplataforma'
      ]
    },
    {
      icon: '🌐',
      title: 'Sites',
      description: 'Desenvolvemos sites responsivos e modernos que representam sua marca e atendem às necessidades do seu negócio. Desde landing pages até portais completos.',
      features: [
        'Design responsivo',
        'Otimização para SEO',
        'Performance e velocidade'
      ]
    }
  ];

  return (
    <section id="servicos" className="section services-section">
      <div className="container">
        <h2 className="section-title">Nossos Serviços</h2>
        <div className="services-grid">
          {services.map((service, index) => (
            <div
              key={index}
              className="service-card"
              ref={el => serviceCardsRef.current[index] = el}
            >
              <div className="service-icon">{service.icon}</div>
              <h3 className="service-title">{service.title}</h3>
              <p className="service-description">{service.description}</p>
              <ul className="service-features">
                {service.features.map((feature, idx) => (
                  <li key={idx}>{feature}</li>
                ))}
              </ul>
            </div>
          ))}
        </div>
      </div>
    </section>
  );
};

export default Services;
