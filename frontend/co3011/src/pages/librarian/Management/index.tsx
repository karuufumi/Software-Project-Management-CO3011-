interface BookDetailProps {
  title: string;
  author: string;
  year: number;
  publisher: string;
  genre: string;
  description: string;
  copies: number;
  rarity: string;
  imageUrl: string;
  onAccept?: () => void;
  onReject?: () => void;
}

export default function LBookDetailx(_: BookDetailProps){

return (
  
  <iframe 
    src="https://striking-crimson-6s4c6cfiy3.edgeone.dev/" 
    width="100%" 
    height="100%" 
    title="Library Dashboard"
></iframe>
)
}
