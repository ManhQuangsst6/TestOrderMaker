import './App.scss';

import RegisterPublicServiceForm from './pages/RegisterPublicServiceForm';
import MedicalDummyDataCreator from './pages/MedicalDummyDataCreator';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
function App() {
  return (
    <Router>
      <Routes>
        <Route exact path="/" element={<RegisterPublicServiceForm />} />
        <Route path="/MedicalDummyDataCreator/:projectNameParam" element={<MedicalDummyDataCreator />} />
      </Routes>
    </Router>
  );
}

export default App;
