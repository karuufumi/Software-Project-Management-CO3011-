import { CustomSlider } from "../../../../../components/slider";
import Widget from "../../../../../components/widget/widget";
import viteLogo from "/vite.svg";

const mockData = [
  {
    title: "1984",
    image: viteLogo,
  },
  {
    title: "To Kill A Mockingbird",
    image: viteLogo,
  },
  {
    title: "The Great Gatsby",
    image: viteLogo,
  },
  {
    title: "Pride and Prejudice",
    image: viteLogo,
  },
  {
    title: "The Catcher in the Rye",
    image: viteLogo,
  },
];

const responsive = {
  superLargeDesktop: {
    // the naming can be any, depends on you.
    breakpoint: { max: 4000, min: 3000 },
    items: 5,
  },
  desktop: {
    breakpoint: { max: 3000, min: 1024 },
    items: 3,
  },
  tablet: {
    breakpoint: { max: 1024, min: 464 },
    items: 2,
  },
  mobile: {
    breakpoint: { max: 464, min: 0 },
    items: 1,
  },
};

export function GeneralReadings() {
  return (
    <>
      <h3
        style={{
          fontWeight: 600,
          textTransform: "capitalize",
          marginBottom: 10,
        }}
      >
        general readings
      </h3>

      <CustomSlider
        carouselProps={{
          responsive: responsive,
          children: mockData.map((item) => (
            <Widget marginLeft={30}>
              <p>{item.title}</p>
              <img src={item.image} className="logo" alt="Vite logo" />
            </Widget>
          )),
          infinite: true,
        }}
        style={{
          paddingBlock: 10,
        }}
      />
    </>
  );
}
