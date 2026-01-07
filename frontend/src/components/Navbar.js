import React, { useState, useEffect } from 'react';

const Navbar = () => {
  const [activeSection, setActiveSection] = useState('inicio');
  const [scrolled, setScrolled] = useState(false);

  useEffect(() => {
    const handleScroll = () => {
      setScrolled(window.scrollY > 100);

      // Atualiza a seção ativa baseado no scroll
      const sections = ['inicio', 'sobre', 'servicos', 'contato'];
      const scrollPosition = window.scrollY + 150;

      sections.forEach(sectionId => {
        const section = document.getElementById(sectionId);
        if (section) {
          const sectionTop = section.offsetTop;
          const sectionHeight = section.clientHeight;

          if (scrollPosition >= sectionTop && scrollPosition < sectionTop + sectionHeight) {
            setActiveSection(sectionId);
          }
        }
      });
    };

    window.addEventListener('scroll', handleScroll);
    return () => window.removeEventListener('scroll', handleScroll);
  }, []);

  const handleNavClick = (e, sectionId) => {
    e.preventDefault();
    const section = document.getElementById(sectionId);
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
    <nav className={`navbar ${scrolled ? 'scrolled' : ''}`}>
      <div className="nav-container">
        <div className="logo">LiSoft</div>
        <ul className="nav-menu">
          <li>
            <a
              href="#inicio"
              className={`nav-link ${activeSection === 'inicio' ? 'active' : ''}`}
              onClick={(e) => handleNavClick(e, 'inicio')}
            >
              Início
            </a>
          </li>
          <li>
            <a
              href="#sobre"
              className={`nav-link ${activeSection === 'sobre' ? 'active' : ''}`}
              onClick={(e) => handleNavClick(e, 'sobre')}
            >
              Sobre
            </a>
          </li>
          <li>
            <a
              href="#servicos"
              className={`nav-link ${activeSection === 'servicos' ? 'active' : ''}`}
              onClick={(e) => handleNavClick(e, 'servicos')}
            >
              Serviços
            </a>
          </li>
          <li>
            <a
              href="#contato"
              className={`nav-link ${activeSection === 'contato' ? 'active' : ''}`}
              onClick={(e) => handleNavClick(e, 'contato')}
            >
              Contato
            </a>
          </li>
        </ul>
      </div>
    </nav>
  );
};

export default Navbar;
